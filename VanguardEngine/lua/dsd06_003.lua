-- 封焔竜 ナモーカール

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.PlacedByRidingOn(obj.GetNameFromCardID("dsd06_004"))
end

function OnPlaceCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.Exists({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dsd06_014"), q.Name, obj.GetNameFromCardID("dsd06_015")})
end

function OnPlace()
	obj.Search({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dsd06_014"), q.Name, obj.GetNameFromCardID("dsd06_015"), q.Count, 1, q.Min, 0})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 2000, p.UntilEndOfBattle)
end