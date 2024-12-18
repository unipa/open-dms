export default function AppearanceGroup(field: any, editField: any): {
    id: string;
    label: string;
    entries: {
        id: string;
        component: (props: any) => any;
        isEdited: any;
        editField: any;
        field: any;
        onChange: (key: any) => (value: any) => void;
        getValue: (key: any) => () => any;
    }[];
};
