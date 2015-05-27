using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderProject.MyMath
{
    public struct Vector2
    {
        public double x, y;

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static implicit operator Vector2(Vector2i v){
            return new Vector2(){
                x = v.x,
                y = v.y
            };
        }
    }

    public struct Vector2i
    {
        public int x, y;

        public static bool operator ==(Vector2i v1, Vector2i v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

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

    }

    public struct Vector3
    {
        public double x, y, z;

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

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
    }

    public struct Vector3i
    {
        public int x, y, z;

        public static bool operator ==(Vector3i v1, Vector3i v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

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
    }
}
