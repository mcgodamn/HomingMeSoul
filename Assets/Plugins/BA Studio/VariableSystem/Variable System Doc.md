# Variable System Doc
This system provide variable storage access for any script in the project.

Insert "using VariableSystem;" to access.


# VariableProfileCache
Type: Monobehaviour

**Attention**: 
As a singleton object, you should only have 1 VariableProfileCache on all gameobjects at the same time.

**Attention**: 
The object attached to will be marked "DontDestroyOnLoad".

**Attention**: 
As a singleton object, Use VariableProfileCache.Instance to access all members.


#### Example Usage
```
VarialbeProfileCache.Instance.SetVar("Player", "Money", 100);
```




## Variable Setters

### void SetVar(string groupID, string key, % value)
- ### Params
    - **groupID**: variable group ID, works as a folder contains variables.
    - **key**: the key of this variable
    - **value**: the value of this variable
        -  % could be float, string or bool
- ### Description
    - Use this method to put your variable into the variable database cache.
    - Does not need to manaully build group, just set your fisrt variable with desired groupID.


<br>

## Variable Getters

### float GetFloat(string groupID, string key)
- #### Params
    - **groupID**: variable group ID, works as a folder contains variables.
    - **key**: the key of this variable
- #### Description
    - Use this method to get your variable from the variable database cache.


### string GetString(string groupID, string key)
- #### Params
    - **groupID**: variable group ID, works as a folder contains variables.
    - **key**: the key of this variable
- #### Description
    - Use this method to get your variable from the variable database cache.


### bool GetBool(string groupID, string key)
- #### Params
    - **groupID**: variable group ID, works as a folder contains variables.
    - **key**: the key of this variable
- #### Description
    - Use this method to get your variable from the variable database cache.


<br>


# VariableManipulation
type: MonoBehaviour
#### Description
Use this component to set variables at runtime with pre-configured parameters.

## Public Methods

### void Execute()
- ### Description
    - Use this method to perform all the configured operations in the component instance.


## Public Properties

### string ID
- ### Description
    - Use this string when multiple VariableManipulation are attached and you need to distinguish.


# VariableCheck
type: MonoBehaviour
#### Description
Use this component to check variables at runtime with pre-configured parameters.
:::info
**Note**
If a condition is set to *Negative*, the check result of the condition will be reversed.
:::

## Public Methods

### void Check()
- ### Description
    - Use this method to check all the configured conditions in the component instance.
    - If conditionGroupType is set to "And", all conditions must be met to trigger the OnConditionMet() event.
    - If conditionGroupType is set to "Or", Any one of the conditions will trigger the OnConditionMet() event.


## Public Properties

### string ID
- ### Description
    - Use this string when multiple VariableCheck are attached and you need to distinguish.


<br>
<br>
<br>

*Short URL of this Doc: https://goo.gl/h3D223*