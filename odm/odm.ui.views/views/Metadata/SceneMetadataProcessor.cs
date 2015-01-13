using onvif.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using utils;

using Win = System.Windows;
using Onvif = onvif.services;

namespace odm.ui.views {
    public class SceneMetadataProcessor {
        readonly Action<VAObjectSnapshot> changed;
        readonly Action<VAObjectSnapshot> initialized;
        readonly Action<VAObjectSnapshot> deleted;

        public SceneMetadataProcessor(Action<VAObjectSnapshot> initialized, Action<VAObjectSnapshot> changed, Action<VAObjectSnapshot> deleted) {
            this.initialized = initialized;
            this.changed = changed;
            this.deleted = deleted;
        }

        //IDictionary<string, Point> objectStartPositions = new Dictionary<string, Point>();

        public void Process(VideoAnalyticsStream analyticsStream) {
            try {
                var items = analyticsStream.items;
                if (items != null) {
                    foreach (var item in items) {
                        if (item is Frame) {
                            var frame = (Frame)item;

                            var time = frame.utcTime; //TODO synchronize displaying frame and its annotation

                            var frameTransformation = frame.transformation;
                            Tuple<Win.Vector, Win.Point> frameTransform = null;
                            if (frameTransformation != null) {
                                frameTransform = Convert(frameTransformation);
                                
                                //TODO hot fix
                                //frameTransform = new Tuple<Win.Vector, Point>(new Win.Vector(0, 0), new Point(1, 1));
                            }

                            var objects = frame.@object;
                            if (objects != null) {
                                foreach (var obj in objects) {
                                    var objId = obj.objectId;
                                    var appearance = obj.appearance;
                                    if (appearance != null) {
                                        var objectTransformation = appearance.transformation;
                                        Tuple<Win.Vector, Win.Point> objTransform = null;
                                        if (objectTransformation != null)
                                            objTransform = Convert(objectTransformation);
                                        if (frameTransform != null && objTransform != null)
                                            objTransform = Transform(frameTransform, objTransform);
                                        else if (frameTransform != null && objTransform == null)
                                            objTransform = frameTransform;

                                        var shape = appearance.shape;
                                        if (shape != null) {
                                            var bb = shape.boundingBox;
                                            var gc = shape.centerOfGravity;

                                            var boundingBox = Convert(bb);
                                            var currentPosition = Convert(gc);

                                            if (objTransform != null) {
                                                boundingBox = Transform(boundingBox, objTransform);
                                                currentPosition = Transform(currentPosition, objTransform);
                                            }

                                            var snapshot = new VAObjectSnapshot() { 
                                                Id = objId,
                                                BoundingBox = boundingBox,
                                                CurrentPosition = currentPosition,
                                                StartPosition = default(Win.Point)
                                            };

                                            //TODO you have to track that some of objects became obsolete
                                            /*Point startPosition;
                                            if (!objectStartPositions.TryGetValue(objId, out startPosition)) {
                                                startPosition = snapshot.CurrentPosition;
                                                objectStartPositions[objId] = startPosition;
                                                snapshot.StartPosition = startPosition;
                                                this.initialized(snapshot);
                                            }
                                            else {
                                                snapshot.StartPosition = startPosition;*/
                                                this.changed(snapshot);
                                            /*}*/
                                        }
                                    }
                                }
                            }

                            var objectTree = frame.objectTree;
                            if (objectTree != null) {
                                var objectIds = objectTree.delete;
                                if (objectIds != null) {
                                    foreach (var objectId in objectIds) {
                                        if (objectId.objectId != null)
                                            this.deleted(new VAObjectSnapshot() { Id = objectId.objectId });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                dbg.Error(ex);
            }
        }

        #region Conversion & Transformation

        Win.Rect Convert(Onvif.Rectangle rect) { 
            return new Rect(new Point(rect.left, rect.bottom), new Point(rect.right, rect.top));
        }
        Win.Point Convert(Onvif.Vector point) {
            return new Point(point.x, point.y);
        }
        Win.Vector ConvertToVec(Onvif.Vector point) {
            return new Win.Vector(point.x, point.y);
        }
        Tuple<Win.Vector, Win.Point> Convert(Onvif.Transformation transform) {
            transform.translate = transform.translate ?? new Onvif.Vector() { x=0,y=0 };
            transform.scale = transform.scale ?? new Onvif.Vector() { x = 1, y = 1 };
            return Tuple.Create(ConvertToVec(transform.translate), Convert(transform.scale));
        }
        Win.Point Transform(Win.Point point, Tuple<Win.Vector, Win.Point> transform){
            return new Win.Point(point.X * transform.Item2.X + transform.Item1.X, point.Y * transform.Item2.Y + transform.Item1.Y);
        }
        Win.Rect Transform(Win.Rect rect, Tuple<Win.Vector, Win.Point> transform) {
            return new Win.Rect(Transform(rect.BottomLeft, transform), Transform(rect.TopRight, transform));
        }
        Tuple<Win.Vector, Win.Point> Transform(Tuple<Win.Vector, Win.Point> parentTransform, Tuple<Win.Vector, Win.Point> childTransform) {
            return Tuple.Create(
                new Win.Vector(childTransform.Item1.X * parentTransform.Item2.X + parentTransform.Item1.X, childTransform.Item1.Y * parentTransform.Item2.Y + parentTransform.Item1.Y),
                new Win.Point(childTransform.Item2.X * parentTransform.Item2.X, childTransform.Item2.Y * parentTransform.Item2.Y)
                );
        }

        #endregion Conversion & Transformation

    }
}
