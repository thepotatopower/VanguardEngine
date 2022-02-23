-- 封焔竜 ハリバドラ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
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
	return obj.PlacedByRidingOn(obj.GetNameFromCardID("dsd06_003"))
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.Drop, q.Other, o.Arms})
end

function OnPlace()
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.Arms, q.Count, 1})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone() and obj.VanguardIsArmed() 
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfBattle)
end
