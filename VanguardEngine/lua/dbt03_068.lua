-- アンブッシュ・キルスモーク

function RegisterAbilities()
	-- blitz order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_002")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfBattle)
	local timedTrigger = NewAbility(GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetCondition("OnBattleEndsCondition")
	timedTrigger.SetActivation("OnBattleEndsActivation")
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
end

function OnBattleEndsCondition()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Other, o.Boosting})
end

function OnBattleEndsActivation()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.Boosting, q.Other, o.CanChoose, q.Count, 1})
end