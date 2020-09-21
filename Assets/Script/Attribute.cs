using Entitas;
public class Game : ContextAttribute { }
public class MyFeature : FeatureAttribute { public MyFeature(int prior = 0) : base(prior) { } }
