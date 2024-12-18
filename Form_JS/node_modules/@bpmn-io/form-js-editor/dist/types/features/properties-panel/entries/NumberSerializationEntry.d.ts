export default function NumberSerializationEntry(props: any): {
    id: string;
    component: typeof SerializeToString;
    isEdited: any;
    editField: any;
    field: any;
}[];
declare function SerializeToString(props: any): any;
export {};
