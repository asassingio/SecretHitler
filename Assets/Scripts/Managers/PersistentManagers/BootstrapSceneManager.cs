using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

namespace Managers.PersistentManagers
{
    public sealed class BootstrapSceneManager : Manager<BootstrapSceneManager>
    {
        public void SetActiveSelf()
        {
            while (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
        
        public void Update()
        {
            if (Input.touchCount == 3)
            {
                Debug.Log("triplo tocco");
                LoadNextOnlineScene();
            }
        }
        
        public void LoadOnlineScene()
        {
            string activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            LoadScene("Scene2");
            UnLoadScene(activeSceneName);
        }

        public void LoadNextOnlineScene()
        {
            string activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; 
            
            if (activeSceneName == "Scene2")
            {
                LoadScene("Scene3");
            }
            else if (activeSceneName == "Scene3")
            {
                LoadScene("Scene2");
            }
            
            UnLoadScene(activeSceneName);
        }
    
        private void LoadScene(string sceneName)
        {
            SceneLoadData sld = new SceneLoadData(sceneName);
            InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        }
    
        private void UnLoadScene(string sceneName)
        {
            SceneUnloadData sld = new SceneUnloadData(sceneName);
            InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
        }
    }
}
