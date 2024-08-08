﻿export const policies = {
  All: 'All',
  AccessManagement: 'AccessManagement',
  User: {
    Me: 'User:Me',
    View: 'User:View',
    UpdateProfile: 'User:UpdateProfile',
    UpdatePassword: 'User:UpdatePassword',
  },
  Role: {
    View: 'Role:View',
    Create: 'Role:Create',
    Update: 'Role:Update',
    Assign: 'Role:Assign',
    Delete: 'Role:Delete',
  },
  Forum: {
    View: 'Forum:View',
    Create: 'Forum:Create',
    Update: 'Forum:Update',
    Delete: 'Forum:Delete',
  },
  Topic: {
    View: 'Topic:View',
    Create: 'Topic:Create',
    Update: 'Topic:Update',
    Delete: 'Topic:Delete',
  },
  Prefix: {
    View: 'Prefix:View',
    Create: 'Prefix:Create',
    Update: 'Prefix:Update',
    Delete: 'Prefix:Delete',
  },
  Post: {
    View: 'Post:View',
    Create: 'Post:Create',
    Update: 'Post:Update',
    Delete: 'Post:Delete',
    Vote: 'Post:Vote',
  },
  Reply: {
    View: 'Reply:View',
    Create: 'Reply:Create',
    Update: 'Reply:Update',
    Delete: 'Reply:Delete',
    Vote: 'Reply:Vote',
  },
};
