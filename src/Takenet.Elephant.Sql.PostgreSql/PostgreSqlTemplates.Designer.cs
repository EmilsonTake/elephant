﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Takenet.Elephant.Sql.PostgreSql {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PostgreSqlTemplates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PostgreSqlTemplates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Takenet.Elephant.Sql.PostgreSql.PostgreSqlTemplates", typeof(PostgreSqlTemplates).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {tableName} ADD {columnDefinition}.
        /// </summary>
        public static string AlterTableAddColumn {
            get {
                return ResourceManager.GetString("AlterTableAddColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to AND.
        /// </summary>
        public static string And {
            get {
                return ResourceManager.GetString("And", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {columnName} {sqlType} NOT NULL.
        /// </summary>
        public static string ColumnDefinition {
            get {
                return ResourceManager.GetString("ColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE {tableName}
        ///( 
        ///{tableDefinition}
        ///).
        /// </summary>
        public static string CreateTable {
            get {
                return ResourceManager.GetString("CreateTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BYTEA.
        /// </summary>
        public static string DbTypeBinary {
            get {
                return ResourceManager.GetString("DbTypeBinary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BOOLEAN.
        /// </summary>
        public static string DbTypeBoolean {
            get {
                return ResourceManager.GetString("DbTypeBoolean", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TIMESTAMP(3).
        /// </summary>
        public static string DbTypeDateTime {
            get {
                return ResourceManager.GetString("DbTypeDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TIMESTAMPTZ(3).
        /// </summary>
        public static string DbTypeDateTimeOffset {
            get {
                return ResourceManager.GetString("DbTypeDateTimeOffset", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DOUBLE PRECISION.
        /// </summary>
        public static string DbTypeDouble {
            get {
                return ResourceManager.GetString("DbTypeDouble", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UUID.
        /// </summary>
        public static string DbTypeGuid {
            get {
                return ResourceManager.GetString("DbTypeGuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SMALLINT.
        /// </summary>
        public static string DbTypeInt16 {
            get {
                return ResourceManager.GetString("DbTypeInt16", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INTEGER.
        /// </summary>
        public static string DbTypeInt32 {
            get {
                return ResourceManager.GetString("DbTypeInt32", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BIGINT.
        /// </summary>
        public static string DbTypeInt64 {
            get {
                return ResourceManager.GetString("DbTypeInt64", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to VARCHAR({length}).
        /// </summary>
        public static string DbTypeString {
            get {
                return ResourceManager.GetString("DbTypeString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM {tableName} WHERE {filter}.
        /// </summary>
        public static string Delete {
            get {
                return ResourceManager.GetString("Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM {tableName} WHERE {filter}; 
        ///INSERT INTO {tableName} ({columns}) 
        ///SELECT {values}
        ///WHERE NOT EXISTS ( SELECT 1 FROM {tableName} WHERE {filter} ).
        /// </summary>
        public static string DeleteAndInsertWhereNotExists {
            get {
                return ResourceManager.GetString("DeleteAndInsertWhereNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @dummy = 0.
        /// </summary>
        public static string DummyEqualsZero {
            get {
                return ResourceManager.GetString("DummyEqualsZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to =.
        /// </summary>
        public static string Equal {
            get {
                return ResourceManager.GetString("Equal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CASE WHEN EXISTS ((SELECT 1 FROM {tableName} WHERE {filter})) THEN CAST(1 AS BOOLEAN) ELSE CAST(0 AS BOOLEAN) END.
        /// </summary>
        public static string Exists {
            get {
                return ResourceManager.GetString("Exists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COLUMN_NAME, CASE WHEN UDT_NAME IN (&apos;varchar&apos;, &apos;timestamp&apos;, &apos;timestamptz&apos;) THEN UDT_NAME ELSE DATA_TYPE END AS DATA_TYPE
        ///FROM INFORMATION_SCHEMA.COLUMNS
        ///WHERE TABLE_NAME = &apos;{tableName}&apos;.
        /// </summary>
        public static string GetTableColumns {
            get {
                return ResourceManager.GetString("GetTableColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &gt;.
        /// </summary>
        public static string GreaterThan {
            get {
                return ResourceManager.GetString("GreaterThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &gt;=.
        /// </summary>
        public static string GreaterThanOrEqual {
            get {
                return ResourceManager.GetString("GreaterThanOrEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {columnName} SERIAL NOT NULL.
        /// </summary>
        public static string IdentityColumnDefinition {
            get {
                return ResourceManager.GetString("IdentityColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IN.
        /// </summary>
        public static string In {
            get {
                return ResourceManager.GetString("In", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO {tableName} ({columns}) VALUES ({values}).
        /// </summary>
        public static string Insert {
            get {
                return ResourceManager.GetString("Insert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO {tableName} ({columns}) 
        ///SELECT {values}
        ///WHERE NOT EXISTS ( SELECT 1 FROM {tableName} WHERE {filter} ).
        /// </summary>
        public static string InsertWhereNotExists {
            get {
                return ResourceManager.GetString("InsertWhereNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {columnName} SMALLSERIAL NOT NULL.
        /// </summary>
        public static string Int16IdentityColumnDefinition {
            get {
                return ResourceManager.GetString("Int16IdentityColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {columnName} SERIAL NOT NULL.
        /// </summary>
        public static string Int32IdentityColumnDefinition {
            get {
                return ResourceManager.GetString("Int32IdentityColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {columnName} BIGSERIAL NOT NULL.
        /// </summary>
        public static string Int64IdentityColumnDefinition {
            get {
                return ResourceManager.GetString("Int64IdentityColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;.
        /// </summary>
        public static string LessThan {
            get {
                return ResourceManager.GetString("LessThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;=.
        /// </summary>
        public static string LessThanOrEqual {
            get {
                return ResourceManager.GetString("LessThanOrEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LIKE.
        /// </summary>
        public static string Like {
            get {
                return ResourceManager.GetString("Like", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO {tableName} ({columns}) VALUES ({values})
        ///ON CONFLICT ({keyColumns}) DO UPDATE SET {columnValues}.
        /// </summary>
        public static string Merge {
            get {
                return ResourceManager.GetString("Merge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NOT.
        /// </summary>
        public static string Not {
            get {
                return ResourceManager.GetString("Not", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;&gt;.
        /// </summary>
        public static string NotEqual {
            get {
                return ResourceManager.GetString("NotEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {columnName} {sqlType} NULL.
        /// </summary>
        public static string NullableColumnDefinition {
            get {
                return ResourceManager.GetString("NullableColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ON {condition}.
        /// </summary>
        public static string On {
            get {
                return ResourceManager.GetString("On", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1 = 1.
        /// </summary>
        public static string OneEqualsOne {
            get {
                return ResourceManager.GetString("OneEqualsOne", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1 = 0.
        /// </summary>
        public static string OneEqualsZero {
            get {
                return ResourceManager.GetString("OneEqualsZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OR.
        /// </summary>
        public static string Or {
            get {
                return ResourceManager.GetString("Or", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CONSTRAINT PK_{tableName} PRIMARY KEY ({columns}).
        /// </summary>
        public static string PrimaryKeyConstraintDefinition {
            get {
                return ResourceManager.GetString("PrimaryKeyConstraintDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {column} = {value}.
        /// </summary>
        public static string QueryEquals {
            get {
                return ResourceManager.GetString("QueryEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {column} &gt; {value}.
        /// </summary>
        public static string QueryGreatherThen {
            get {
                return ResourceManager.GetString("QueryGreatherThen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {column} &lt; {value}.
        /// </summary>
        public static string QueryLessThen {
            get {
                return ResourceManager.GetString("QueryLessThen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT {columns} FROM {tableName} WHERE {filter}.
        /// </summary>
        public static string Select {
            get {
                return ResourceManager.GetString("Select", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM {tableName} WHERE {filter}.
        /// </summary>
        public static string SelectCount {
            get {
                return ResourceManager.GetString("SelectCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT {columns} FROM {tableName} WHERE {filter} ORDER BY {orderBy} LIMIT {take} OFFSET {skip}.
        /// </summary>
        public static string SelectSkipTake {
            get {
                return ResourceManager.GetString("SelectSkipTake", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT {columns} FROM {tableName} WHERE {filter} LIMIT 1.
        /// </summary>
        public static string SelectTop1 {
            get {
                return ResourceManager.GetString("SelectTop1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CASE WHEN EXISTS ((SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = &apos;{tableName}&apos;)) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END.
        /// </summary>
        public static string TableExists {
            get {
                return ResourceManager.GetString("TableExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE {tableName}
        ///SET {columnValues}
        ///WHERE {filter}.
        /// </summary>
        public static string Update {
            get {
                return ResourceManager.GetString("Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {value} AS {column}.
        /// </summary>
        public static string ValueAsColumn {
            get {
                return ResourceManager.GetString("ValueAsColumn", resourceCulture);
            }
        }
    }
}
