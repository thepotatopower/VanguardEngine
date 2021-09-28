-- Blaze Fist Monk, Damari

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.FrontRowEnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 1, q.Min, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBattleEnds, p.HasPrompt, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(1)
		if obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") then
			obj.SuperiorCall(2)
		end
	end
	return 0
end
