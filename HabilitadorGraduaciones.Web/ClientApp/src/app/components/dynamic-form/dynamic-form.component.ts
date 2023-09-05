import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormInputBase } from '../../classes/form-input-base';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.css']
})
export class DynamicFormComponent  {
  @Input() input!: FormInputBase<string>;
  @Input() form!: FormGroup;
}
