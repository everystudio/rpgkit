using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpgkit{
    public class TeleportTo : MonoBehaviour
    {
        public string teleport_name;
        public string target_scene;
        public string target_teleport;
        public TeleportEntry entry;

        void Start()
        {
            if(entry != null)
            {
                entry = transform.GetComponentInChildren<TeleportEntry>();
            }
            entry.teleport_name = teleport_name;
            
            if( FieldManager.Instance.TargetTeleporterName == teleport_name)
            {
                // 初期配置が必要な場合
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                // 移動先の保存
                FieldManager.Instance.TargetTeleporterName = target_teleport;

                // 現在は演出なしで行う
                SceneManager.LoadScene(target_scene);

                /*
                // フェードアウトさせる処理
                StartCoroutine(ScreenEffect.Instance.Fadeout(1.0f, () =>
                {
                   SceneManager.LoadScene(target_scene);
                }));
                */

                // 完了後にシーンをロードする
            }
        }


    }
}