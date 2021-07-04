-- Hyperspeed Robo, Chevalstud

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerPrisoners, q.Count, 3
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and not obj.Activated() and obj.CanCB(1) and obj.CanSB(2) and obj.Exists(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	obj.CounterBlast(1)
	obj.SoulBlast(2)
end

function Activate(n)
	if n == 1 then
		obj.ChooseImprison(3)
		if obj.Exists(4) then
			obj.Draw(1)
		end
	end
	return 0
end