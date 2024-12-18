export default function ConstraintsGroup(field: any, editField: any): {
    id: string;
    label: string;
    entries: {
        id: string;
        component: (props: any) => any;
        isEdited: any;
        editField: any;
        field: any;
    }[];
};
