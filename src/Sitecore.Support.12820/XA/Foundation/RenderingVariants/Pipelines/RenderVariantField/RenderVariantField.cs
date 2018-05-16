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
      Control variantFieldNameLiteral;
      if (string.IsNullOrEmpty(variantField.FieldName))
      {
        return new LiteralControl();
      }
      #region patch 12820
      //Changed condition to check if we are editing in Experience Editor
      if ((this.IsEmptyFieldToRender(variantField, args.Item) || this.IsFromSnippedAndEmpty(variantField, args.Item, args.IsControlEditable)) && !(Context.PageMode.IsExperienceEditorEditing))
      {
        return new LiteralControl();
      }
      #endregion
      if (args.Item.Fields[variantField.FieldName] != null)
      {
        variantFieldNameLiteral = this.CreateFieldRenderer(variantField, args.Item, args.IsControlEditable, args.IsFromComposite);
      }
      else if (args.IsControlEditable && Context.PageMode.IsExperienceEditorEditing)
      {
        variantFieldNameLiteral = this.GetVariantFieldNameLiteral(variantField);
      }
      else
      {
        return new LiteralControl();
      }
      variantField.IsLink = this.ProtectLink(variantField.FieldName, variantField.IsLink, args.Item);
      variantFieldNameLiteral = this.HandleAffixAndLinkCreation(variantFieldNameLiteral, args.Item, variantField.Prefix, variantField.Suffix, variantField.IsLink, variantField.IsDownloadLink, variantField.IsPrefixLink, variantField.IsSuffixLink, variantField.LinkAttributes, variantField.LinkField, args.HrefOverrideFunc);
      if (!string.IsNullOrWhiteSpace(variantField.Tag))
      {
        HtmlGenericControl tag = new HtmlGenericControl(variantField.Tag);
        this.AddClass(tag, $"{variantField.CssClass} {this.GetFieldCssClass(variantField.FieldName)}".Trim());
        this.AddWrapperDataAttributes(variantField, args, tag);
        this.MoveControl(variantFieldNameLiteral, tag);
        variantFieldNameLiteral = tag;
      }
      return variantFieldNameLiteral;
    }
  }
}