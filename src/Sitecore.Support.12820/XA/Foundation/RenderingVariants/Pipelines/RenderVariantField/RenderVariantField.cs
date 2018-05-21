using Sitecore.XA.Foundation.RenderingVariants.Fields;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.RenderVariantField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Sitecore.Support.XA.Foundation.RenderingVariants.Pipelines.RenderVariantField
{
  public class RenderVariantField: Sitecore.XA.Foundation.RenderingVariants.Pipelines.RenderVariantField.RenderVariantField
  {
    protected override Control Render(VariantField variantField, RenderVariantFieldArgs args)
    {
      if (string.IsNullOrEmpty(variantField.FieldName))
      {
        return new LiteralControl();
      }
      #region patch 12820
      if (this.IsEmptyFieldToRender(variantField, args.Item) && !(Context.PageMode.IsExperienceEditorEditing && args.IsControlEditable))
      {
        return new LiteralControl();
      }
      #endregion patch 12820
      Control control;
      if (args.Item.Fields[variantField.FieldName] != null)
      {
        control = this.CreateFieldRenderer(variantField, args.Item, args.IsControlEditable, args.IsFromComposite);
      }
      else
      {
        if (!args.IsControlEditable || !this.PageMode.IsExperienceEditorEditing)
        {
          return new LiteralControl();
        }
        control = this.GetVariantFieldNameLiteral(variantField);
      }
      variantField.IsLink = this.ProtectLink(variantField.FieldName, variantField.IsLink, args.Item);
      control = this.HandleAffixAndLinkCreation(control, args.Item, variantField.Prefix, variantField.Suffix, variantField.IsLink, variantField.IsDownloadLink, variantField.IsPrefixLink, variantField.IsSuffixLink, variantField.LinkAttributes, variantField.LinkField, args.HrefOverrideFunc);
      if (!string.IsNullOrWhiteSpace(variantField.Tag))
      {
        HtmlGenericControl htmlGenericControl = new HtmlGenericControl(variantField.Tag);
        this.AddClass(htmlGenericControl, string.Format("{0} {1}", variantField.CssClass, this.GetFieldCssClass(variantField.FieldName)).Trim());
        this.AddWrapperDataAttributes(variantField, args, htmlGenericControl);
        this.MoveControl(control, htmlGenericControl);
        control = htmlGenericControl;
      }
      return control;
    }

  }
}