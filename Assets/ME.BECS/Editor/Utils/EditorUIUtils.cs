namespace ME.BECS.Editor {
    
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;
    using UnityEditor;

    public static class EditorUIUtils {
        
        public static uint ValidateMin(DropdownField dropdown, IntegerField field) {

            var minSizeInKb = (int)(MemoryAllocator.MIN_ZONE_SIZE / 1024);
            
            var c = dropdown.index + 1;
            if (c == 1 && field.value < minSizeInKb) {
                field.value = minSizeInKb;
            }
            
            if (c == 2 && field.value <= 0) {
                field.value = minSizeInKb;
                dropdown.index = 0;
                c = 1;
            }
            
            return EditorUtils.IntToBytes(field.value, c);

        }

        public static void DrawBytesField(VisualElement foldout, SerializedProperty property) {

            var choices = new System.Collections.Generic.List<string>() {
                "KB",
                "MB",
            };
            
            var container = new VisualElement();
            container.AddToClassList("field");
            foldout.Add(container);
            
            var so = property.serializedObject;
            var prop = property;
            var nameField = new IntegerField(prop.displayName);
            nameField.value = EditorUtils.BytesToInt(prop.uintValue, out var cat);
            var dropdown = new DropdownField(choices, cat - 1);
            nameField.RegisterValueChangedCallback((evt) => {
                so.Update();
                var bytes = ValidateMin(dropdown, nameField);
                prop.uintValue = bytes;
                so.ApplyModifiedProperties();
                so.Update();
            });
            container.Add(nameField);

            dropdown.RegisterValueChangedCallback((evt) => {
                so.Update();
                prop.uintValue = ValidateMin(dropdown, nameField);
                so.ApplyModifiedProperties();
                so.Update();
            });
            container.Add(dropdown);
            
            ValidateMin(dropdown, nameField);
            
            var tooltip = property.tooltip;
            if (string.IsNullOrEmpty(tooltip) == false) {

                tooltip = tooltip.Replace("{MIN_ZONE_SIZE_IN_KB}", MemoryAllocator.MIN_ZONE_SIZE_IN_KB.ToString());

                container.AddToClassList("has-tooltip");
                var tooltipElement = new Label($"<b>{property.displayName}</b>\n{tooltip}");
                tooltipElement.AddToClassList("tooltip-text");
                tooltipElement.pickingMode = PickingMode.Ignore;
                var tooltipButton = new Label("?");
                tooltipButton.AddToClassList("tooltip");
                container.Add(tooltipElement);
                container.Add(tooltipButton);

            }

        }

        public static void DrawTooltip(VisualElement container, SerializedProperty property) {
            
            var tooltip = property.tooltip;
            if (string.IsNullOrEmpty(tooltip) == false) {

                DrawTooltip(container, $"<b>{property.displayName}</b>\n{tooltip}");

            }

        }

        public static VisualElement DrawTooltip(VisualElement container, string tooltip) {
            
            if (string.IsNullOrEmpty(tooltip) == false) {

                container.AddToClassList("has-tooltip");
                var tooltipElement = new Label(tooltip);
                tooltipElement.AddToClassList("tooltip-text");
                tooltipElement.pickingMode = PickingMode.Ignore;
                var tooltipButton = new Label("?");
                tooltipButton.AddToClassList("tooltip");
                container.Add(tooltipElement);
                container.Add(tooltipButton);
                return tooltipElement;

            }

            return null;

        }

        public static void DrawPropertyField(VisualElement root, SerializedProperty property) {
            
            var container = new VisualElement();
            container.AddToClassList("field");
            root.Add(container);

            var prop = new UnityEditor.UIElements.PropertyField(property.Copy());
            prop.BindProperty(property.Copy());
            container.Add(prop);

            DrawTooltip(container, property);
            
        }

        public static void DrawUIntField(VisualElement foldout, SerializedProperty property, int minValue = 0) {

            var container = new VisualElement();
            container.AddToClassList("field");
            foldout.Add(container);
            
            var so = property.serializedObject;
            var prop = property;
            var nameField = new IntegerField(prop.displayName);
            nameField.value = (int)prop.uintValue;
            nameField.RegisterValueChangedCallback((evt) => {
                so.Update();
                var val = evt.newValue;
                if (val <= minValue) val = minValue;
                prop.uintValue = (uint)val;
                nameField.value = val;
                so.ApplyModifiedProperties();
                so.Update();
            });
            container.Add(nameField);
            
            DrawTooltip(container, property);

        }

        public static void DrawToggleField(VisualElement foldout, SerializedProperty property) {

            var container = new VisualElement();
            container.AddToClassList("field");
            foldout.Add(container);
            
            var so = property.serializedObject;
            var prop = property;
            var nameField = new Toggle(prop.displayName);
            nameField.value = prop.boolValue;
            nameField.RegisterValueChangedCallback((evt) => {
                so.Update();
                var val = evt.newValue;
                prop.boolValue = val;
                nameField.value = val;
                so.ApplyModifiedProperties();
                so.Update();
            });
            container.Add(nameField);
            
            DrawTooltip(container, property);

        }

    }

}