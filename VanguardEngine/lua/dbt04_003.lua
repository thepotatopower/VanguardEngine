-- トリクムーン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Drop)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable() and obj.Exists({q.Location, l.PlayerRC, q.Other, o.ThisFieldID, q.FL, FL.PlayerBackCenter})
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown})
end

function OnPlace()
	obj.CounterCharge(1)
end

function OnAttackTrigger()
	return obj.IsBooster()
end

function OnAttackCondition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd06_001")}) and obj.GetArmsCount() > 0
end

function OnAttack()
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd06_001")}) then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 10000 * obj.GetArmsCount(), p.UntilEndOfBattle)
	end
end