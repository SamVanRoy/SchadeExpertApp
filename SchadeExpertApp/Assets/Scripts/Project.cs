using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if NETFX_CORE
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class Project : MonoBehaviour {
#if NETFX_CORE
    public StorageFolder ProjectProperty { get; set; }
#endif
    public string NameSchadeExpert { get; set; }
}
