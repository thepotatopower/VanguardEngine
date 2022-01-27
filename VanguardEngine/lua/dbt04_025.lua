-- 双連の大魔法 トトーネ

function RegisterAbilities()
	-- on battle ends
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Attacking})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.RevealedTriggers, q.Trigger, tt.Critical, q.Trigger, tt.Front})
end

function Activation()
	if obj.Exists({q.Location, l.RevealedTriggers, q.Trigger, tt.Critical}) then
		obj.ChooseSendToBottom({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	end
	if obj.Exists({q.Location, l.RevealedTriggers, q.Trigger, tt.Front}) then
		obj.ChooseStand({q.Location, l.PlayerRC, q.Grade, 2, q.Other, o.GradeOrLess, q.Count, 1})
	end
end