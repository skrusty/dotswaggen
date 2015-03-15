using System;
using System.Collections.Generic;
using System.Linq;

namespace dotswaggen.Swagger
{
    public class DataTypeRegistry
    {
        public enum CommonNames
        {
            INTEGER,
            LONG,
            FLOAT,
            DOUBLE,
            STRING,
            BYTE,
            BOOLEAN,
            DATE,
            DATETIME
        }

        public enum Formats
        {
            INT32,
            INT64,
            FLOAT,
            DOUBLE,
            BYTE,
            DATE,
            DATETIME
        }

        public enum Types
        {
            INTEGER,
            NUMBER,
            STRING,
            BOOLEAN
        }

        public static void WithPrimitiveType(string type, string format, Action<CommonNames> toDo)
        {
            Types maybeType;
            Formats? maybeFormat;
            Formats formatHolder;

            if (!Enum.TryParse(type, true, out maybeType))
                return; //Unrecognized type

            if (format == null || !Enum.TryParse(format, true, out formatHolder))
            {
                maybeFormat = null;
            }
            else
            {
                maybeFormat = formatHolder;
            }

            var foundTypes =
                PrimitiveTypes.Where(t => t.Item2.Item1 == maybeType && t.Item2.Item2 == maybeFormat).ToArray();
            if (foundTypes.Length == 1)
                toDo(foundTypes[0].Item1);
        }

        private static Tuple<Types, Formats?> Pair(Types t, Formats f)
        {
            return new Tuple<Types, Formats?>(t, f);
        }

        private static Tuple<Types, Formats?> Pair(Types t)
        {
            return new Tuple<Types, Formats?>(t, null);
        }

        public static readonly List<Tuple<CommonNames, Tuple<Types, Formats?>>> PrimitiveTypes = new List
            <Tuple<CommonNames, Tuple<Types, Formats?>>>
        {
            Tuple.Create(CommonNames.INTEGER, Pair(Types.INTEGER, Formats.INT32)),
            Tuple.Create(CommonNames.LONG, Pair(Types.INTEGER, Formats.INT64)),
            Tuple.Create(CommonNames.FLOAT, Pair(Types.NUMBER, Formats.FLOAT)),
            Tuple.Create(CommonNames.DOUBLE, Pair(Types.NUMBER, Formats.DOUBLE)),
            Tuple.Create(CommonNames.BYTE, Pair(Types.STRING, Formats.BYTE)),
            Tuple.Create(CommonNames.DATE, Pair(Types.STRING, Formats.DATE)),
            Tuple.Create(CommonNames.DATETIME, Pair(Types.STRING, Formats.DATETIME)),
            Tuple.Create(CommonNames.INTEGER, Pair(Types.INTEGER)),
            Tuple.Create(CommonNames.DOUBLE, Pair(Types.NUMBER)),
            Tuple.Create(CommonNames.STRING, Pair(Types.STRING)),
            Tuple.Create(CommonNames.BOOLEAN, Pair(Types.BOOLEAN))
        };
    }
}