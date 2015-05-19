﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Takenet.Elephant.Sql.Mapping;

namespace Takenet.Elephant.Sql
{
    internal static class DatabaseSchema
    {
        internal static async Task CreateTableAsync(IDatabaseDriver databaseDriver, DbConnection connection, ITable table, CancellationToken cancellationToken)
        {
            if (table.Columns.Count == 0)
            {
                throw new InvalidOperationException("The table mapper has no defined columns");
            }

            if (!table.KeyColumnsNames.Any())
            {
                throw new InvalidOperationException("The table mapper has no defined key columns");
            }

            // Table columns
            var createTableSqlBuilder = new StringBuilder();
            createTableSqlBuilder.AppendLine(GetColumnsDefinitionSql(databaseDriver, table, table.Columns));

            // Constraints
            createTableSqlBuilder.AppendLine(
                databaseDriver.GetSqlStatementTemplate(SqlStatement.PrimaryKeyConstraintDefinition).Format(
                    new
                    {
                        tableName = table.Name,
                        columns = table.KeyColumnsNames.Select(c => c.AsSqlIdentifier()).ToCommaSeparate()
                    })
            );

            // Create table 
            var createTableSql = databaseDriver.GetSqlStatementTemplate(SqlStatement.CreateTable).Format(
                new
                {
                    tableName = table.Name.AsSqlIdentifier(),
                    tableDefinition = createTableSqlBuilder.ToString()
                });

            await connection.ExecuteNonQueryAsync(
                createTableSql,
                cancellationToken).ConfigureAwait(false);

        }

        internal static async Task UpdateTableSchemaAsync(IDatabaseDriver databaseDriver, DbConnection connection, ITable table, CancellationToken cancellationToken)
        {
            var tableColumnsDictionary = new Dictionary<string, string>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = databaseDriver.GetSqlStatementTemplate(SqlStatement.GetTableColumns).Format(
                    new
                    {
                        tableName = table.Name
                    });

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        tableColumnsDictionary.Add((string)reader[0], (string)reader[1]);
                    }
                }
            }

            var columnsToBeCreated = new List<KeyValuePair<string, SqlType>>();

            foreach (var column in table.Columns)
            {
                // Check if the column exists in the database
                if (!tableColumnsDictionary.ContainsKey(column.Key))
                {
                    columnsToBeCreated.Add(column);
                }
                // Checks if the existing column type matches with the definition
                // The comparion is with startsWith for the NVARCHAR values
                else if (!GetSqlTypeSql(databaseDriver, column.Value).StartsWith(
                         tableColumnsDictionary[column.Key], StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("The existing column '{columnName}' type '{columnType}' is not compatible with the definition type '{dbType}'".Format(new { columnName = column.Key, columnType = tableColumnsDictionary[column.Key], dbType = column.Value }));
                }
            }

            if (columnsToBeCreated.Any())
            {
                await CreateColumnsAsync(databaseDriver, connection, table, columnsToBeCreated, cancellationToken);
            }
        }

        private static async Task CreateColumnsAsync(IDatabaseDriver databaseDriver, DbConnection connection, ITable table, IEnumerable<KeyValuePair<string, SqlType>> columns, CancellationToken cancellationToken)
        {
            var command = connection.CreateCommand();
            command.CommandText = databaseDriver.GetSqlStatementTemplate(SqlStatement.AlterTableAddColumn).Format(
                new
                {
                    tableName = table.Name,
                    columnDefinition = GetColumnsDefinitionSql(databaseDriver, table, columns).TrimEnd(',')
                });

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        private static string GetColumnsDefinitionSql(IDatabaseDriver databaseDriver, ITable table, IEnumerable<KeyValuePair<string, SqlType>> columns)
        {
            var columnSqlBuilder = new StringBuilder();

            foreach (var column in columns)
            {
                // All columns, except the key are nullable
                if (table.KeyColumnsNames.Contains(column.Key))
                {
                    if (column.Value.IsIdentity)
                    {
                        columnSqlBuilder.AppendLine(
                            databaseDriver.GetSqlStatementTemplate(SqlStatement.IdentityColumnDefinition).Format(
                                new
                                {
                                    columnName = column.Key.AsSqlIdentifier(),
                                    sqlType = GetSqlTypeSql(databaseDriver, column.Value)
                                })
                            );
                    }
                    else
                    {
                        columnSqlBuilder.AppendLine(
                            databaseDriver.GetSqlStatementTemplate(SqlStatement.ColumnDefinition).Format(
                                new
                                {
                                    columnName = column.Key.AsSqlIdentifier(),
                                    sqlType = GetSqlTypeSql(databaseDriver, column.Value)
                                })
                            );
                    }
                }
                else
                {
                    columnSqlBuilder.AppendLine(
                        databaseDriver.GetSqlStatementTemplate(SqlStatement.NullableColumnDefinition).Format(
                            new
                            {
                                columnName = column.Key.AsSqlIdentifier(),
                                sqlType = GetSqlTypeSql(databaseDriver, column.Value)
                            })
                        );
                }

                columnSqlBuilder.Append(",");
            }
            return columnSqlBuilder.ToString();
        }

        private static string GetSqlTypeSql(IDatabaseDriver databaseDriver, SqlType sqlType)
        {
            var typeSql = databaseDriver.GetSqlTypeName(sqlType.Type);

            if (sqlType.Length.HasValue)
            {
                string lengthValue = sqlType.Length == int.MaxValue ? SqlType.MAX_LENGTH : sqlType.Length.ToString();
                typeSql = typeSql.Format(new
                {
                    length = lengthValue
                });
            }

            if (sqlType.Precision.HasValue)
            {
                typeSql = typeSql.Format(new
                {
                    precision = sqlType.Precision
                });
            }

            if (sqlType.Scale.HasValue)
            {
                typeSql = typeSql.Format(new
                {
                    scale = sqlType.Scale
                });
            }

            return typeSql;
        }
    }
}