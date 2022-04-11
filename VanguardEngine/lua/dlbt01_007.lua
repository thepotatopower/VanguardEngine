-- 巡り星の綺想曲 イングリット

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetNotMovedFrom(l.Hand)
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
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown}) or 
	(obj.IsSameZone() and obj.Exists({q.Location, l.PlayerRC, q.Other, o.Resting, q.Other, o.SameColumn}))
end

function OnPlace()
	obj.CounterCharge(1)
	obj.ChooseStand({q.Location, l.PlayerRC, q.Other, o.Resting, q.Other, o.SameColumn})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfBattle)
	if obj.IsSameZone() then
		local timedTrigger = GiveAbility(GetID(), GetID())
		timedTrigger.SetTiming(a.OnBattleEnds)
		timedTrigger.SetActivation("TimedTrigger")
		timedTrigger.SetResetTarget(GetID())
		timedTrigger.SetResetTiming(p.UntilEndOfBattle)
	end
end

function TimedTrigger()
	obj.SendToBottom({q.Other, o.ThisFieldID})
end
