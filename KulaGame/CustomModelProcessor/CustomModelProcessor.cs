//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content.Pipeline;
//using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
//using Microsoft.Xna.Framework.Content.Pipeline.Processors;

//namespace CustomModelProcessor
//{
//    [ContentProcessor]
//    public class CustomModelProcessor : ModelProcessor
//    {
//        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
//        {
//            float minX = float.MaxValue;
//            float minY = float.MaxValue;
//            float minZ = float.MaxValue;
//            float maxX = float.MinValue;
//            float maxY = float.MinValue;
//            float maxZ = float.MinValue;
//            foreach (NodeContent nc in input.Children)
//            {
//                if (nc is MeshContent)
//                {
//                    MeshContent mc = (MeshContent)nc;
//                    foreach (Vector3 basev in mc.Positions)
//                    {
//                        Vector3 v = basev;
//                        if (v.X < minX)
//                            minX = v.X;

//                        if (v.Y < minY)
//                            minY = v.Y;

//                        if (v.Z < minZ)
//                            minZ = v.Z;

//                        if (v.X > maxX)
//                            maxX = v.X;

//                        if (v.Y > maxY)
//                            maxY = v.Y;

//                        if (v.Z > maxZ)
//                            maxZ = v.Z;
//                    }
//                }
//            }
//            ModelContent mc2 = base.Process(input, context);
//            mc2.Tag = new BoundingBox(new Vector3((float)minX, (float)minY, (float)minZ), new Vector3((float)maxX, (float)maxY, (float)maxZ));
//            return mc2;
//        }
//    }
//}