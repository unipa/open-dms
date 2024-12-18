declare class FormEditorKeyboardBindings {
    constructor(eventBus: any, keyboard: any);
    registerBindings(keyboard: any, editorActions: any): void;
}
declare namespace FormEditorKeyboardBindings {
    const $inject: string[];
}
export default FormEditorKeyboardBindings;
