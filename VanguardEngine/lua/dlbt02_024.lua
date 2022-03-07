-- 永遠に別たぬ夜明曲 イレーネ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerRC, q.Race, r.Ghost, q.UnitType, u.Normal, q.Other, o.Applicable})
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 2000, p.UntilEndOfBattle)
	local timedTrigger = GiveAbility(GetID(), GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
	timedTrigger.SetCondition("TimedTriggerCondition")
	timedTrigger.SetActivation("TimedTrigger")
	local IDs = obj.GetIDs({q.Location, l.PlayerRC, q.Race, r.Ghost, q.UnitType, u.Normal, q.Other, o.Applicable})
	timedTrigger.StoreMultiple(IDs)
end

function TimedTriggerCondition()
	return obj.GetNumberOf("Filter", {l.Units}) > 0
end

function Filter(id)
	return obj.IsStored(id)
end

function TimedTrigger()
	if obj.YesNo(obj.GetDescription(2)) then
		obj.SendToBottom("Filter", {l.Units})
	end
end