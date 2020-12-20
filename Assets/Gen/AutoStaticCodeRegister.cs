namespace PuertsStaticWrap
{
    public static class AutoStaticCodeRegister
    {
        public static void Register(Puerts.JsEnv jsEnv)
        {
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.GameObject), UnityEngine_GameObject_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(GameElementImageView), GameElementImageView_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(GameElementView), GameElementView_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(EliminateGameController), EliminateGameController_Wrap.GetRegisterInfo);
            
        }
    }
}