-- Stealth Dragon, Kizanreiji

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Other, o.SameColumn, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanRetire(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(1)
	end
	return 0
end