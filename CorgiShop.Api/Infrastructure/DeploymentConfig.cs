namespace CorgiShop.Api.Infrastructure
{
    public enum DeploymentConfigType
    {
        LocalDev,
        LocalDocker
    }

    public static class DeploymentConfig
    {
        public static DeploymentConfigType Configuration
        {
            get
            {
#if LOCALDOCKER
            return DeploymentConfigType.LocalDocker;
#endif
                return DeploymentConfigType.LocalDev;
            }
        }
    }
}
