-- Tearful Malice

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 2
	elseif n == 2 then
		return q.Location, l.PlayedOrder
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.Retire, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanRetire(1) then 
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

function Cost(n)
	if n == 1 then
		obj.ChooseRetire(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.AddToSoul(2)
		obj.CounterCharge(1)
	end
	return 0
end