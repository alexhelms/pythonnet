using System;

namespace Python.Runtime
{
    /// <summary>
    /// Represents a Python tuple object. See the documentation at
    /// PY2: https://docs.python.org/2/c-api/tupleObjects.html
    /// PY3: https://docs.python.org/3/c-api/tupleObjects.html
    /// for details.
    /// </summary>
    public class PyTuple : PySequence
    {
        /// <summary>
        /// PyTuple Constructor
        /// </summary>
        /// <remarks>
        /// Creates a new PyTuple from an existing object reference. Note
        /// that the instance assumes ownership of the object reference.
        /// The object reference is not checked for type-correctness.
        /// </remarks>
        public PyTuple(IntPtr ptr) : base(ptr)
        {
        }


        /// <summary>
        /// PyTuple Constructor
        /// </summary>
        /// <remarks>
        /// Copy constructor - obtain a PyTuple from a generic PyObject. An
        /// ArgumentException will be thrown if the given object is not a
        /// Python tuple object.
        /// </remarks>
        public PyTuple(PyObject o)
        {
            if (!IsTupleType(o))
            {
                throw new ArgumentException("object is not a tuple");
            }
            Runtime.XIncref(o.obj);
            obj = o.obj;
        }


        /// <summary>
        /// PyTuple Constructor
        /// </summary>
        /// <remarks>
        /// Creates a new empty PyTuple.
        /// </remarks>
        public PyTuple()
        {
            obj = Runtime.PyPyTuple_New(0);
            if (obj == IntPtr.Zero)
            {
                throw new PythonException();
            }
        }


        /// <summary>
        /// PyTuple Constructor
        /// </summary>
        /// <remarks>
        /// Creates a new PyTuple from an array of PyObject instances.
        /// </remarks>
        public PyTuple(PyObject[] items)
        {
            int count = items.Length;
            obj = Runtime.PyPyTuple_New(count);
            for (var i = 0; i < count; i++)
            {
                IntPtr ptr = items[i].obj;
                Runtime.XIncref(ptr);
                int r = Runtime.PyPyTuple_SetItem(obj, i, ptr);
                if (r < 0)
                {
                    throw new PythonException();
                }
            }
        }


        /// <summary>
        /// IsTupleType Method
        /// </summary>
        /// <remarks>
        /// Returns true if the given object is a Python tuple.
        /// </remarks>
        public static bool IsTupleType(PyObject value)
        {
            return Runtime.PyPyTuple_Check(value.obj);
        }


        /// <summary>
        /// AsTuple Method
        /// </summary>
        /// <remarks>
        /// Convert a Python object to a Python tuple if possible, raising
        /// a PythonException if the conversion is not possible. This is
        /// equivalent to the Python expression "tuple(object)".
        /// </remarks>
        public static PyTuple AsTuple(PyObject value)
        {
            IntPtr op = Runtime.PyPySequence_Tuple(value.obj);
            if (op == IntPtr.Zero)
            {
                throw new PythonException();
            }
            return new PyTuple(op);
        }
    }
}
