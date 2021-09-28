-- Explosive! Melting Heart!

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Location, l.EnemyVC, q.Other, o.Attacking, q.Other, o.CanChoose, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerPrisoners
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(2) > 0 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(1, obj.GetNumberOf(2) * -5000)
	end
	return 0
end