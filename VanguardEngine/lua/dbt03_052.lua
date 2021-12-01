-- ハイレートバースト・ドラゴン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_009")})
end

function OnPlace()
	obj.Select({q.Location, l.PlayerRC, q.Other, o.NotThisFieldID, q.Count, 1})
	if obj.GetNumberOf({q.Location, l.Selected}) > 0 then
		local id = obj.GetID({q.Location, l.Selected})
		local given = GiveAbility(GetID(), id)
		given.SetDescription(2)
		given.SetTiming(a.OnAttack)
		given.SetTrigger("OnAttackTrigger")
		given.SetLocation(l.RC)
		given.SetCondition("OnAttackCondition")
		given.SetCost("OnAttackCost")
		given.SetCanFullyResolve("OnAttackCanFullyResolve")
		given.SetActivation("OnAttack")
		given.SetResetTarget(id)
		given.SetResetTiming(p.UntilEndOfTurn)
	end
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.NumOfAttacks() == 1 
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID, q.Other, o.Resting})
end

function OnAttack()
	obj.Stand({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end