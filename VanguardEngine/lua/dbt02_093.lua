-- Explosive! Melting Heart!

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Location, l.EnemyVC, q.Other, o.Attacking, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerPrisoners
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanSB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(3) > 0 then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(2, obj.GetNumberOf(3) * -5000)
	end
	return 0
end