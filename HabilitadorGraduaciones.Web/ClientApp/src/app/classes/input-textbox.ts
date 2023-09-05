import { FormInputBase } from "./form-input-base";
export class TextboxInput extends FormInputBase<string> {
    override controlType = 'textbox';
  }