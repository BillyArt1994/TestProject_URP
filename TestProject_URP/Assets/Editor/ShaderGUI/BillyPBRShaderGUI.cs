using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace CommonShaderGUI
{
    class BillyPBRShaderGUI : BillyShaderGUI
    {

        public override void FindProperties(MaterialProperty[] props)
        {
            base.FindProperties(props);
        }

        public override void OnOpenGUI()
        {
            base.OnOpenGUI();
        }

        public override void RefreshKeywords()
        {
            base.RefreshKeywords();
        }
    }
}

