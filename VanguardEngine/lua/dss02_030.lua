-- 涙滴賦活の女魔術師

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.GetNumberOf("Filter", {l.EnemyUnits}) > 0 and obj.IsSameZone()
end

function Filter(id)
	return obj.IsAttacker(id) and obj.Critical(id) >= 2 and (obj.OriginalCritical(id) >= 2 or obj.CriticalIncreasedByAbility(id))
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusShield, 15000, p.UntilEndOfBattle)
end