import { Component, Input, OnDestroy, OnInit, forwardRef } from '@angular/core';
import { FormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Editor, NgxEditorComponent, NgxEditorMenuComponent, Toolbar } from 'ngx-editor';

@Component({
  selector: 'app-description-editor',
  imports: [
    NgxEditorComponent,
    NgxEditorMenuComponent,
    FormsModule
  ],
  templateUrl: './description-editor.component.html',
  styleUrl: './description-editor.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DescriptionEditorComponent),
      multi: true
    }
  ]
})
export class DescriptionEditorComponent implements OnInit, OnDestroy, ControlValueAccessor {
  @Input() placeholder: string = '';

  editor: Editor = new Editor();
  toolbar: Toolbar = [
    ['bold', 'italic', 'underline', 'strike'],
    ['code', 'blockquote'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['text_color', 'background_color'],
    ['align_left', 'align_center', 'align_right', 'align_justify'],
    ['horizontal_rule', 'format_clear', 'indent', 'outdent'],
    ['superscript', 'subscript'],
    ['undo', 'redo'],
  ];

  private onChange = (value: any) => {};
  private onTouched = () => {};
  public value = '';

  ngOnInit(): void {
    this.editor.setContent(this.value);
  }

  ngOnDestroy(): void {
    this.editor?.destroy();
  }

  writeValue(value: any): void {
    this.value = value || '';
    if (this.editor) {
      this.editor.setContent(this.value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    // Implement if needed
  }

  onContentChange(value: string): void {
    this.value = value;
    this.onChange(value);
    this.onTouched();
  }
}
