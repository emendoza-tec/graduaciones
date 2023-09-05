import { FormInputBase } from "./form-input-base";

export class DropDownInput extends FormInputBase<string> {
    override controlType = 'dropdown';
  }