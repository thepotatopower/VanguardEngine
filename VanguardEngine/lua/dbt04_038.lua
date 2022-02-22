-- デザイアデビル ワルズーレ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetLocation(l.Soul)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player, q.Name, obj.GetNameFromCardID("dbt03_003")})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone()
end

function Activation()
	obj.SuperiorCall({q.Other, o.ThisFieldID})
end