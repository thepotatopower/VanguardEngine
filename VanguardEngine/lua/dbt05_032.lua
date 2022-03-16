-- 禁覧の魔女 エルンマス

function RegisterAbilities()
	-- on retire
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRetire)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.SourceIsVanguardAbility() and obj.SourceNameContains(obj.LoadName("Blaster"))
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose})
end

function Activation()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end
