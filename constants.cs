using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubernetesUtil
{
    public static class RuleTypes
    {
      public const  string OPA_RULE_MISSING = "1. Kindly add Open Policy Agent (OPA) agent add-on to the cluster !";
      public const string CERTIFICATE_MISSING = "2. Kindly use tls for security in the cluster!";
      public const string NETWORK_POLICY_MISSING = "3. There are no network policies defined in the cluster!";
      public const string RESOURCE_QUOTA_MISSING= "4. There are no resource quota defined in the cluster!";
    }
}
