// See https://aka.ms/new-console-template for more information
using Azure.ResourceManager;
using k8s;
using Azure.ResourceManager.Resources;
using Azure.Identity;
using System.Runtime.CompilerServices;
using Azure.Identity;

namespace KubernetesUtil
{
    internal class Program
    {
       
        private static async Task Main(string[] args)
        {
            int opa_count = 0;
            var config = KubernetesClientConfiguration.BuildDefaultConfig();

            ArmClient clientp = new ArmClient(new DefaultAzureCredential());
            SubscriptionResource subscription = clientp.GetDefaultSubscription();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Kindly hold on for few secs, analysing cluster for checking best practises rule set");

            //var credentials = new AzureCredentials(new UserLoginInformation { ClientId = "Azure client Id", UserName = "username", Password = "Password" }, "tenant Id", AzureEnvironment.AzureGlobalCloud);  //AzureChinaCloud,AzureGermanCloud,AzureUSGovernment

           
            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("This utility can be used to validate a k8s cluster for best practises ...............");

            // Check for OPA add-on installed or not - RULE 1
            var list = client.CoreV1.ListNamespace();
            foreach (var item in list.Items)
            {
                if (item.Metadata.Name == "gatekeeper-system")
                {
                    opa_count++;
                }
                
            }

            if (opa_count <= 0)
            { 
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(KubernetesUtil.RuleTypes.OPA_RULE_MISSING + Environment.NewLine);
            }

            // Check for client certificates for the apiserver - RULE 2
            var check2 = client.Certificates;
            if(check2== null )
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(KubernetesUtil.RuleTypes.CERTIFICATE_MISSING + Environment.NewLine);
            }

           
            // Check for network policies - RULE 3
            var netpol = client.NetworkingV1.ListNamespacedNetworkPolicy("default");
            if(netpol.Items.Count<=0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(KubernetesUtil.RuleTypes.NETWORK_POLICY_MISSING + Environment.NewLine);
            }


            // Check for network policies - RULE 4
            var resource_quota = client.CoreV1.ListNamespacedResourceQuota("default");
            if(resource_quota !=null && resource_quota.Items.Count==0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(KubernetesUtil.RuleTypes.RESOURCE_QUOTA_MISSING + Environment.NewLine);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Currently only 3 rules defined but it can be extended to fit n number of rules!");
            Console.WriteLine("Press enter to exit !");
            Console.ReadLine();
            
        }



    }

}