using System;

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

        public static implicit operator Vector2(Vector2i v){
            return new Vector2(){
                x = v.x,
                y = v.y
            };
        }

        public static implicit operator Vector2(Vector3 v){
            return new Vector2(){
                x = v.x,
                y = v.y
            };
        }

        public static implicit operator Vector2(Vector3i v){
            return new Vector2(){
                x = v.x,
                y = v.y
            };
        }

        #endregion

    }

    public struct Vector2i
    {
        public int x, y;
        
        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Vector2i v1, Vector2i v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector2i v1, Vector2i v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2i)) return false;
            return (Vector2i)obj == this;
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

        public static implicit operator Vector2i(Vector2 v){
            return new Vector2i(){
                x = (int)v.x,
                y = (int)v.y
            };
        }

        public static implicit operator Vector2i(Vector3 v){
            return new Vector2i(){
                x = (int)v.x,
                y = (int)v.y
            };
        }

        public static implicit operator Vector2i(Vector3i v){
            return new Vector2i(){
                x = (int)v.x,
                y = (int)v.y
            };
        }

        #endregion

        #region Counstructors

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

    }

    public struct Vector3
    {

        private const double NormalTolerance = 0.00000001;

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

        public static implicit operator Vector3(Vector2 v){
            return new Vector3(){
                x = v.x,
                y = v.y,
                z = 0
            };
        }

        public static implicit operator Vector3(Vector2i v){
            return new Vector3(){
                x = v.x,
                y = v.y,
                z = 0
            };
        }

        public static implicit operator Vector3(Vector3i v){
            return new Vector3(){
                x = v.x,
                y = v.y,
                z = v.z
            };
        }

        public static implicit operator Vector3(Quaternion v){
            return new Vector3(){
                x = v.x,
                y = v.y,
                z = v.z
            };
        }

        #endregion

        #region Constructors
        
        public Vector3(double x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }

        public Vector3(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
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
            double x1fn = v2.x - v1.x;
            double y1fn = v2.y - v1.y;
            double z1fn = v2.z - v1.z;

            double x2fn = v3.x - v1.x;
            double y2fn = v3.y - v1.y;
            double z2fn = v3.z - v1.z;
            double nx = y1fn * z2fn - z1fn * y2fn;
            double ny = z1fn * x2fn - x1fn * z2fn;
            double nz = x1fn * y2fn - y1fn * x2fn;
            return new Vector3(nx, ny, nz);
        }

        #endregion

        #region Public functions
        
        public void Normalize()
        {
            double module = x*x + y*y + z*z;
            if (Math.Abs(module - 1) < NormalTolerance) return;
            module = Math.Sqrt(module);
            x = (float)(x / module);
            y = (float)(y / module);
            z = (float)(z / module);
        }

        #endregion

    }

    public struct Vector3i
    {
        public int x, y, z;
        
        #region ==, !=, Equals, GetHashCode, ToString

        public static bool operator ==(Vector3i v1, Vector3i v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3i v1, Vector3i v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z == v2.z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3i)) return false;
            return (Vector3i)obj == this;
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

        public static implicit operator Vector3i(Vector2 v){
            return new Vector3i(){
                x = (int)v.x,
                y = (int)v.y,
                z = 0
            };
        }

        public static implicit operator Vector3i(Vector2i v){
            return new Vector3i(){
                x = v.x,
                y = v.y,
                z = 0
            };
        }

        public static implicit operator Vector3i(Vector3 v){
            return new Vector3i(){
                x = (int)v.x,
                y = (int)v.y,
                z = (int)v.z
            };
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

        public static implicit operator Quaternion(Vector2 v){
            return new Quaternion(){
                x = v.x,
                y = v.y,
                z = 0,
                w = 0
            };
        }

        public static implicit operator Quaternion(Vector2i v){
            return new Quaternion(){
                x = v.x,
                y = v.y,
                z = 0,
                w = 0
            };
        }

        public static implicit operator Quaternion(Vector3i v){
            return new Quaternion(){
                x = v.x,
                y = v.y,
                z = v.z,
                w = 0
            };
        }

        #endregion

        #region Constructors
        
        public Quaternion(double x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }

        public Quaternion(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
            this.w = 0;
        }

        public Quaternion(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0;
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
