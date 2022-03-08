-- 注がれる期待 イルタ

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
	return obj.IsApplicable() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Clarissa")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	if obj.IsSameZone() then
		local timedTrigger = GiveAbility(GetID(), GetID())
		timedTrigger.SetTiming(a.OnBattleEnds)
		timedTrigger.SetResetTarget(GetID())
		timedTrigger.SetResetTiming(p.UntilEndOfBattle)
		timedTrigger.SetActivation("TimedTrigger")
	end
end

function TimedTrigger()
	obj.SendToBottom({q.Other, o.ThisFieldID})
end