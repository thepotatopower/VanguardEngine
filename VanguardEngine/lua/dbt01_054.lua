-- Grief, Despair, and Rejection

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then 
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 3
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.AlchemagicCB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanCB(1) and obj.VanguardIs("Mysterious Rain Spiritualist, Zorga") then 
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
		obj.CounterBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(2, 10000)
	end
	return 0
end