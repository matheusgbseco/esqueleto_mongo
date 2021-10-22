﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Gen
{
    public class HelperSysObjectsVue : HelperSysObjectsBaseFront
    {


        public HelperSysObjectsVue(Context context)
            : this(context, "Templates\\Front")
        { }

        public HelperSysObjectsVue(Context context, string template)
        {
            PathOutputBase.UsePathProjects = true;

            this.Contexts = new List<Context> { context };
            this._defineTemplateFolder = new DefineTemplateFolder();
            this._defineTemplateFolder.SetTemplatePathBase(template);
        }

        public HelperSysObjectsVue(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutputBase.UsePathProjects = true;
        }

        public override void DefineTemplateByTableInfoFields(Context configContext, TableInfo tableInfo, UniqueListInfo infos)
        {
            base.DefineTemplateByTableInfoFields(configContext, tableInfo, infos);

            ExecuteTemplateVueFormCreateComponent(tableInfo, configContext, infos);
            ExecuteTemplateVueFormEditComponent(tableInfo, configContext, infos);
            ExecuteTemplateVueIndex(tableInfo, configContext, infos);
        }

        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo)
        {
            ExecuteTemplateVueCreateComponent(tableInfo, config);
            ExecuteTemplateVueEditComponent(tableInfo, config);
            ExecuteTemplateVueRouter(tableInfo, config);
        }

        private void ExecuteTemplateVueCreateComponent(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputVue.PathOutputVueViewComponent(tableInfo, configContext, "create");
            if (File.Exists(pathOutput)) return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueCreateComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput, false, Encoding.UTF8)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateVueEditComponent(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputVue.PathOutputVueViewComponent(tableInfo, configContext, "edit");
            if (File.Exists(pathOutput)) return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueEditComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput, false, Encoding.UTF8)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateVueFormCreateComponent(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented) return;
            if (!tableInfo.MakeFront) return;

            var pathOutput = PathOutputVue.PathOutputVueViewComponent(tableInfo, configContext, "form-create");
            if (File.Exists(pathOutput)) return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueCreateFormComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeFormClassBuilder(tableInfo, configContext, infos, textTemplate, "model");

            using (var stream = new StreamWriter(pathOutput, false, Encoding.UTF8)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateVueFormEditComponent(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented) return;
            if (!tableInfo.MakeFront) return;

            var pathOutput = PathOutputVue.PathOutputVueViewComponent(tableInfo, configContext, "form-edit");
            if (File.Exists(pathOutput)) return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueEditFormComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeFormClassBuilder(tableInfo, configContext, infos, textTemplate, "model");

            using (var stream = new StreamWriter(pathOutput, false, Encoding.UTF8)) { stream.Write(classBuilder); }
        }


        private void ExecuteTemplateVueRouter(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputVue.PathOutputVueRouterViewComponent(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueRouterComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var itens = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                var str1 = @"    { path: '" + item.ClassName.ToLower() + "', name: '" + (item.ClassNameFormated ?? item.ClassName) + "', component: () => import('@/views/" + item.ClassName.ToLower() + "/" + item.ClassName.ToLower() + "-index') },";
                var str2 = @"    { path: '" + item.ClassName.ToLower() + "/create', name: 'Create " + (item.ClassNameFormated ?? item.ClassName) + "', component: () => import('@/views/" + item.ClassName.ToLower() + "/" + item.ClassName.ToLower() + "-create') },";
                var str3 = @"    { path: '" + item.ClassName.ToLower() + "/edit/:id', name: 'Edit " + (item.ClassNameFormated ?? item.ClassName) + "', component: () => import('@/views/" + item.ClassName.ToLower() + "/" + item.ClassName.ToLower() + "-edit') },";
                itens += string.Format("{0}{1}", str1, System.Environment.NewLine);
                itens += string.Format("{0}{1}", str2, System.Environment.NewLine);
                itens += string.Format("{0}{1}", str3, System.Environment.NewLine);
            }

            classBuilder = classBuilder.Replace("<#pathsRoute#>", itens);

            using (var stream = new StreamWriter(pathOutput, false, Encoding.UTF8)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateVueIndex(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputVue.PathOutputVueViewComponent(tableInfo, configContext, "index");
            if (File.Exists(pathOutput)) return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueIndexComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);
            classBuilder = this.MakeGridClassBuilder(tableInfo, configContext, infos, classBuilder);
            classBuilder = this.MakeFilterClassBuilder(tableInfo, configContext, infos, classBuilder);
            using (var stream = new StreamWriter(pathOutput, false, Encoding.UTF8)) { stream.Write(classBuilder); }
        }

        private string MakeFormClassBuilder(TableInfo tableInfo, Context configContext, UniqueListInfo infos, string textTemplate, string modelType)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var classBuilderForm = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    var itemForm = string.Empty;
                    var pathTemplateField = string.Empty;
                    var textTemplateField = string.Empty;

                    var fieldInBlackListCreate = FieldInBlackListSave(tableInfo, item.PropertyName);
                    if (fieldInBlackListCreate)
                        continue;

                    if (item.IsKey == 1 && !IsPropertyInstance(tableInfo, item.PropertyName))
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldHidden(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else if (item.Type == "string")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldInput(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else if (item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldDate(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }
                    else if (item.Type == "bool")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldCheckbox(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }
                    else if (item.Type == "bool?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldCheckbox(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else
                    {
                        if (IsPropertyInstance(tableInfo, item.PropertyName))
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldSelect(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                        else
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldInput(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                    }

                    var pathTemplateRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueRequired(tableInfo));
                    var textTemplateRequired = Read.AllText(tableInfo, pathTemplateRequired, this._defineTemplateFolder);

                    itemForm = FormFieldReplace(configContext, tableInfo, item, textTemplateField);
                    itemForm = itemForm
                        .Replace("<#modelType#>", modelType)
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#isRequired#>", item.IsNullable == 0 ? textTemplateRequired : "")
                        .Replace("<#isRequiredLabelTag#>", item.IsNullable == 0 ? "" : "")
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName))
                        .Replace("<#propertyNameLowerCase#>", CamelCaseTransform(item.PropertyName));

                    classBuilderForm += string.Format("{0}{1}", itemForm, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#formFields#>", classBuilderForm);
            return classBuilder;
        }

        private string MakeFilterClassBuilder(TableInfo tableInfo, Context configContext, UniqueListInfo infos, string textTemplate)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var classBuilderForm = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    if (item.IsKey == 1 && !IsPropertyInstance(tableInfo, item.PropertyName))
                        continue;

                    var itemForm = string.Empty;
                    var pathTemplateField = string.Empty;
                    var textTemplateField = string.Empty;

                    var fieldInBlackListFilter = FieldInBlackListFilter(tableInfo, item.PropertyName);
                    if (fieldInBlackListFilter)
                        continue;

                    if (item.Type == "string")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldInput(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else if (item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldDate(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);

                        itemForm.Replace("<#propertyName#>", string.Format("{0}{1}{2}", item.PropertyName + "Start", System.Environment.NewLine, "<#propertyName#>"));
                        itemForm.Replace("<#propertyName#>", string.Format("{0}", item.PropertyName + "End"));
                    }
                    else if (item.Type == "bool" || item.Type == "bool?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldRadio(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else
                    {
                        if (IsPropertyInstance(tableInfo, item.PropertyName))
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldSelect(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                        else
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueFieldInput(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                    }

                    tableInfo.TwoCols = true;

                    itemForm = FormFieldReplace(configContext, tableInfo, item, textTemplateField);
                    itemForm = itemForm
                        .Replace("<#modelType#>", "filter")
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#propertyNameLowerCase#>", CamelCaseTransform(item.PropertyName))
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#isRequired#>", "")
                        .Replace("<#isRequiredLabelTag#>", "")
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    classBuilderForm += string.Format("{0}{1}", itemForm, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#formFieldsFilter#>", classBuilderForm);
            return classBuilder;
        }

        private string MakeGridClassBuilder(TableInfo tableInfo, Context configContext, UniqueListInfo infos, string textTemplate)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var classBuilderThead = string.Empty;
            var classBuilderTbody = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    var itemForm = string.Empty;
                    var textTemplateThead = string.Empty;
                    var textTemplateTbody = string.Empty;

                    var fieldInBlackListCreate = FieldInBlackListSave(tableInfo, item.PropertyName);
                    if (fieldInBlackListCreate)
                        continue;

                    if (item.IsKey == 1 && !IsPropertyInstance(tableInfo, item.PropertyName))
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTheadId(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTbodyString(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else if (item.Type == "string")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTbodyString(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else if (item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTbodyDate(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }
                    else if (item.Type == "bool" || item.Type == "bool?")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTbodyBoolean(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else if (item.Type == "decimal" || item.Type == "decimal?" || item.Type == "float" || item.Type == "float?")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTbodyNumber(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else
                    {
                        if (!IsPropertyInstance(tableInfo, item.PropertyName))
                        {
                            var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTheadId(tableInfo));
                            textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                            var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameVue.VueTbodyString(tableInfo));
                            textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    textTemplateThead = textTemplateThead
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#propertyNameLowerCase#>", CamelCaseTransform(item.PropertyName))
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    textTemplateTbody = textTemplateTbody
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#propertyNameLowerCase#>", CamelCaseTransform(item.PropertyName))
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    classBuilderThead += string.Format("{0}{1}", textTemplateThead, System.Environment.NewLine);
                    classBuilderTbody += string.Format("{0}{1}", textTemplateTbody, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder
                .Replace("<#theadFields#>", classBuilderThead)
                .Replace("<#tbodyFields#>", classBuilderTbody);
            return classBuilder;
        }


        #region transformation

        protected override string CamelCaseTransform(string str)
        {
            var ex = this._camelCasingExceptions.Where(_ => _.ToUpper() == str.ToUpper()).SingleOrDefault();
            if (ex.IsNotNull())
                return ex;

            return str.FisrtCharToLower();
        }

        public override string TransformFieldString(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldDateTime(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate, bool onlyDate = false)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldBool(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldPropertyInstance(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldHtml(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }


        public override string TransformFieldUpload(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldTextStyle(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldTextEditor(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        public override string TransformFieldTextTag(ConfigExecutetemplate configExecutetemplate, Info info, string propertyName, string textTemplate)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
