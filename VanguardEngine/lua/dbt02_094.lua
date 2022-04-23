-- 蝕まれる月光

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
	-- on put
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(1)
	ability2.SetTiming(a.OnPut)
	ability2.SetMovedTo(l.Order, l.Player)
	ability2.SetTrigger("OnPutTrigger")
	ability2.SetActivation("OnPut")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
end

function OnPutTrigger()
	return obj.IsApplicable()
end

function OnPut()
	obj.SuperiorCall({}, {}, 1, 1, "dbt01_t01", {}, {})
end