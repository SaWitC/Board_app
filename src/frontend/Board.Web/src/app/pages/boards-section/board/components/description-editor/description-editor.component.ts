import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormsModule, FormGroup } from '@angular/forms';
import { Editor, NgxEditorComponent, NgxEditorMenuComponent, Toolbar } from 'ngx-editor';
import { ReactiveFormsModule, FormControl } from '@angular/forms';

@Component({
  selector: 'app-description-editor',
  imports: [
    NgxEditorComponent,
    NgxEditorMenuComponent,
    FormsModule,
    ReactiveFormsModule
],
  templateUrl: './description-editor.component.html',
  styleUrl: './description-editor.component.scss',
})
export class DescriptionEditorComponent implements
  OnInit,
  OnDestroy
{
  @Input() placeholder: string = '';
  @Input() formControl!: FormControl;

  editor: Editor = new Editor();
  toolbar: Toolbar = [
    ['bold', 'italic', 'underline', 'strike'],
    ['code', 'blockquote'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link', 'image'],
    ['text_color', 'background_color'],
    ['align_left', 'align_center', 'align_right', 'align_justify'],
    ['horizontal_rule', 'format_clear', 'indent', 'outdent'],
    ['superscript', 'subscript'],
    ['undo', 'redo'],
  ];

  ngOnInit(): void {
    if (this.formControl) {
      this.editor.setContent(this.formControl.value || '');
    }
  }

  ngOnDestroy(): void {
    this.editor?.destroy();
  }
}
