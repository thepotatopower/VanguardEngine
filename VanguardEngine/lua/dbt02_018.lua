-- 柩機の兵 サンボリーノ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Other, o.Attacking})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 15000, p.UntilEndOfTurn)
	local timedTrigger = GiveAbility(GetID(), GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetActivation("TimedTrigger")
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
	timedTrigger.StoreMultiple(obj.GetIDs({q.Other, o.ThisFieldID}, {q.Location, l.PlayerUnits, q.Other, o.Attacking}))
end

function TimedTrigger()
	obj.Retire({q.Location, l.Stored})
end