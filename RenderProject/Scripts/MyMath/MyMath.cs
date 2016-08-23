namespace RenderProject.MyMath
{
    public struct Vector2
    {

        public double x, y;

        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2)) return false;
            return (Vector2)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Vector2(" + x + ", " + y + ")";
        }

        #endregion

        #region Convertors

        public static implicit operator Vector2(Vector2I v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator Vector2(Vector3I v)
        {
            return new Vector2(v.x, v.y);
        }

        #endregion

        #region Constructors

        public Vector2(double x) : this(x, 0)
        {
        }

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

    }

    public struct Vector2I
    {
        public int x, y;

        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Vector2I v1, Vector2I v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector2I v1, Vector2I v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2I)) return false;
            return (Vector2I)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Vector2i(" + x + ", " + y + ")";
        }

        #endregion

        #region Convertors

        public static implicit operator Vector2I(Vector2 v)
        {
            return new Vector2I((int)v.x, (int)v.y);
        }

        public static implicit operator Vector2I(Vector3 v)
        {
            return new Vector2I((int)v.x, (int)v.y);
        }

        public static implicit operator Vector2I(Vector3I v)
        {
            return new Vector2I(v.x, v.y);
        }

        #endregion

        #region Counstructors

        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

    }

    public struct Vector3
    {

        private const double NORMAL_TOLERANCE = 0.00000001;

        public double x, y, z;

        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3)) return false;
            return (Vector3)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Vector3(" + x + ", " + y + ", " + z + ")";
        }

        #endregion

        #region Convertors

        public static implicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        public static implicit operator Vector3(Vector2I v)
        {
            return new Vector3(v.x, v.y);
        }

        public static implicit operator Vector3(Vector3I v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static implicit operator Vector3(Quaternion v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        #endregion

        #region Constructors

        public Vector3(double x) : this(x, 0, 0)
        {
        }

        public Vector3(double x, double y) : this(x, y, 0)
        {
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Public static functions

        public static Vector3 CountNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            double x1Fn = v2.x - v1.x;
            double y1Fn = v2.y - v1.y;
            double z1Fn = v2.z - v1.z;

            double x2Fn = v3.x - v1.x;
            double y2Fn = v3.y - v1.y;
            double z2Fn = v3.z - v1.z;
            double nx = y1Fn * z2Fn - z1Fn * y2Fn;
            double ny = z1Fn * x2Fn - x1Fn * z2Fn;
            double nz = x1Fn * y2Fn - y1Fn * x2Fn;
            return new Vector3(nx, ny, nz);
        }

        #endregion

        #region Public functions

        public void Normalize()
        {
            double module = x * x + y * y + z * z;
            if (System.Math.Abs(module - 1) < NORMAL_TOLERANCE) return;
            module = System.Math.Sqrt(module);
            x = (float)(x / module);
            y = (float)(y / module);
            z = (float)(z / module);
        }

        #endregion

    }

    public struct Vector3I
    {
        public int x, y, z;

        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Vector3I v1, Vector3I v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3I v1, Vector3I v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z == v2.z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3I)) return false;
            return (Vector3I)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Vector3i(" + x + ", " + y + ", " + z + ")";
        }

        #endregion

        #region Convertors

        public static implicit operator Vector3I(Vector2 v)
        {
            return new Vector3I((int)v.x, (int)v.y);
        }

        public static implicit operator Vector3I(Vector2I v)
        {
            return new Vector3I(v.x, v.y);
        }

        public static implicit operator Vector3I(Vector3 v)
        {
            return new Vector3I((int)v.x, (int)v.y, (int)v.z);
        }

        #endregion

        #region Constructors

        public Vector3I(int x) : this(x, 0 ,0)
        {
        }

        public Vector3I(int x,int y) : this(x,y,0)
        {
        }

        public Vector3I(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

    }

    public struct Quaternion
    {
        public double x, y, z, w;

        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Quaternion v1, Quaternion v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;
        }

        public static bool operator !=(Quaternion v1, Quaternion v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion)) return false;
            return (Quaternion)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Quaternion(" + x + ", " + y + ", " + z + ", " + w + ")";
        }

        #endregion

        #region Convertors

        public static implicit operator Quaternion(Vector2 v)
        {
            return new Quaternion(v.x, v.y);
        }

        public static implicit operator Quaternion(Vector2I v)
        {
            return new Quaternion(v.x, v.y);
        }

        public static implicit operator Quaternion(Vector3I v)
        {
            return new Quaternion(v.x, v.y, v.z);
        }

        #endregion

        #region Constructors

        public Quaternion(double x) : this(x, 0, 0, 0)
        {
        }

        public Quaternion(double x, double y) : this(x, y, 0, 0)
        {
        }

        public Quaternion(double x, double y, double z) : this(x, y, z, 0)
        {
        }

        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #endregion
    }
}
